/**
 * By default, Remix will handle hydrating your app on the client for you.
 * You are free to delete this file if you'd like to, but if you ever want it revealed again, you can run `npx remix reveal` ✨
 * For more information, see https://remix.run/file-conventions/entry.client
 */

import { RemixBrowser } from "@remix-run/react";
import { startTransition, StrictMode } from "react";
import { hydrateRoot } from "react-dom/client";
import LanguageDetector from "i18next-browser-languagedetector";
import Backend from "i18next-http-backend";

import { CacheProvider } from "@emotion/react";
import createCache from "@emotion/cache";
import i18n from "./i18n";
import i18next from "i18next";
import { I18nextProvider, initReactI18next } from "react-i18next";
import { getInitialNamespaces } from "remix-i18next";
import { getCookie } from "./getCookie";

export const cspNode = document.querySelector<HTMLMetaElement>(
  'meta[name="csp-nonce"]'
);
const cspNonce = cspNode?.content;
cspNode?.setAttribute("content", "__NONCE__");

async function hydrate() {
  // eslint-disable-next-line import/no-named-as-default-member
  await i18next
    .use(initReactI18next) // Tell i18next to use the react-i18next plugin
    .use(LanguageDetector) // Setup a client-side language detector
    .use(Backend) // Setup your backend
    .init({
      ...i18n, // spread the configuration
      // This function detects the namespaces your routes rendered while SSR use
      debug: true,
      ns: getInitialNamespaces(),
      backend: { loadPath: "/locales/{{lng}}/{{ns}}.json" },
      detection: {
        // Here only enable htmlTag detection, we'll detect the language only
        // server-side with remix-i18next, by using the `<html lang>` attribute
        // we can communicate to the client the language detected server-side
        order: ["htmlTag"],
        // Because we only use htmlTag, there's no reason to cache the language
        // on the browser, so we disable it
        caches: [],
      },
    });

  const originalFetch = window.fetch;

  // Override the global fetch function
  window.fetch = async (input, init = {}) => {
    const location = window.location.host;
    const isSameOrigin =
      input instanceof Request
        ? new URL(input.url).host === location
        : new URL(input).host === location;

    if (!isSameOrigin) {
      return originalFetch(input, init);
    }
    // Ensure init.headers is an object if not already set
    if (!init.headers) {
      init.headers = {};
    }

    // Check if headers is an instance of Headers
    if (init.headers instanceof Headers) {
      // Use Headers.append() to add your custom header
      init.headers.append("x-csrf-token", getCookie("csrf-token"));
    } else {
      // Add your custom header
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      (init.headers as any)["x-csrf-token"] = getCookie("csrf-token");
    }

    // Call the original fetch function with the modified init object
    return originalFetch(input, init);
  };

  startTransition(() => {
    hydrateRoot(
      document,

      <I18nextProvider i18n={i18next}>
        <StrictMode>
          <CacheProvider
            value={createCache({ key: "style-cache", nonce: cspNonce })}
          >
            <RemixBrowser />
          </CacheProvider>
        </StrictMode>
      </I18nextProvider>
    );
  });
}
if (window.requestIdleCallback) {
  window.requestIdleCallback(hydrate);
} else {
  // Safari doesn't support requestIdleCallback
  // https://caniuse.com/requestidlecallback
  window.setTimeout(hydrate, 1);
}
