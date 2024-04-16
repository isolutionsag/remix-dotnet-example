import type {
  ActionFunctionArgs,
  LinksFunction,
  LoaderFunctionArgs,
} from "@remix-run/node";
import {
  Links,
  LiveReload,
  Meta,
  Outlet,
  Scripts,
  ScrollRestoration,
  json,
  useLoaderData,
} from "@remix-run/react";

import styles from "./root.css";
import i18next from "./i18n.server";
import type { User } from "./models/User";

import { useTranslation } from "react-i18next";
import Layout from "./components/layout/Layouts";
import { useContext } from "react";
import { NonceContext } from "./components/NonceContext";
import { UserContextProvider } from "./components/contexts/UserContext";
import { LoginLogoutSection } from "./components/layout/LoginLogoutSection";
import { authenticator } from "./services/auth.server";
import { produceCsrfLoaderResponse } from "./services/csrf.server";

export const links: LinksFunction = () => [{ rel: "stylesheet", href: styles }];

export async function action({ request }: ActionFunctionArgs) {
  await authenticator.logout(request, {
    redirectTo: "/",
  });
}

export async function loader({ request }: LoaderFunctionArgs) {
  const locale = await i18next.getLocale(request);
  const userWithTokens = await authenticator.isAuthenticated(request);
  const user: User | null =
    userWithTokens != null
      ? {
          displayname: userWithTokens.displayname,
          firstname: userWithTokens.firstname,
          lastname: userWithTokens.lastname,
          username: userWithTokens.username,
          roles: userWithTokens.roles,
        }
      : null;
  return user == null
    ? json({ user, locale })
    : produceCsrfLoaderResponse(request, { user, locale });
}

export const handle = {
  // In the handle export, we can add a i18n key with namespaces our route
  // will need to load. This key can be a single string or an array of strings.
  // TIP: In most cases, you should set this to your defaultNS from your i18n config
  // or if you did not set one, set it to the i18next default namespace "translation"

  // why do we need all the namespaces? only general and layout should be needed!!
  i18n: ["general", "layout", "list", "listentries", "category", "loginLogout"],
};

export default function App() {
  const { i18n } = useTranslation();
  const { user, locale } = useLoaderData<typeof loader>();
  const nonce = useContext(NonceContext);

  return (
    <html lang={locale} dir={i18n.dir()}>
      <head>
        <meta charSet="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <meta
          name="csp-nonce"
          content={typeof document === "undefined" ? "__NONCE__" : nonce}
        />
        <Meta />
        <Links />
        {typeof document === "undefined" ? "__STYLES__" : null}
      </head>
      <body>
        <UserContextProvider user={user}>
          <Layout>
            <LoginLogoutSection />
            <Outlet />
            <ScrollRestoration nonce={nonce} />
            <Scripts nonce={nonce} />
            <LiveReload nonce={nonce} />
          </Layout>
        </UserContextProvider>
      </body>
    </html>
  );
}
