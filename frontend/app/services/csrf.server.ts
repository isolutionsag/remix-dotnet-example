import crypto from "crypto";
import { getSession } from "./session.server";
import { json } from "@remix-run/react";

export const produceCsrfLoaderResponse = async <T extends object>(
  request: Request,
  data: T
) => {
  const session = await getSession(request.headers.get("Cookie"));
  const sessionId = session.id;

  const secret = process.env.CSRF_COOKIE_SIGNING_KEYS?.split(',')[0] ?? "";
  const randomValue = crypto.randomUUID();

  const message = `${sessionId}!${randomValue}`;  
  const hmac = crypto
    .createHmac("sha256", secret)
    .update(message)
    .digest("hex");
  const csrfToken = `${message}.${hmac}`;

  // standard stuff
  return json(data, {
    headers: [
      [
        "set-cookie",
        "csrf-token=" + btoa(csrfToken) + "; Secure; SameSite=Lax; Path=/",
      ],
    ] as HeadersInit,
  });
};
