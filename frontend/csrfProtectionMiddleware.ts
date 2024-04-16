import { RequestHandler } from "express";
import crypto from "crypto";

export const csrfMiddleware: RequestHandler = (req, res, next) => {
  if (req.method === "GET" && req.header("sec-fetch-mode") === "navigate") {
    // for navigation requests, validating CSRF makes no sense.
    // We need to make sure that page navigation is never modifiying data though
    next();
    return;
  }
  const sessionCookie = req.cookies["__session"];
  if (!sessionCookie) {
    // user is not logged in, no need to check CSRF
    next();
    return;
  }
  // no need to verify the signature, if the signature is invalid, Remix will reject the session anyway
  const sessionCookieValue = sessionCookie.split(".")[0];

  // decode the session cookie by reversing the encoding that Remix applies
  const sessionId = JSON.parse(atob(decodeURI(sessionCookieValue)));

  let csrfCookie = "";
  let csrfHeader = "";
  try {
    csrfCookie = atob(req.cookies["csrf-token"]);
    csrfHeader = atob((req.headers["x-csrf-token"] as string) ?? "");
  } catch (e) {
    res.status(400).send("Could not decode CSRF token");
    console.error(
      "CSRF DECODE ERROR",
      req.cookies["csrf-token"],
      req.headers["x-csrf-token"]
    );
    return;
  }

  if (!csrfCookie || !csrfHeader || csrfCookie !== csrfHeader) {
    res.status(400).send("missing CSRF token");
    console.error("CSRF ERROR");
    return;
  }

  // Decode the CSRF token and check if it matches the session ID. Also check if the signature is valid

  const message = csrfCookie.split(".")[0];
  const receivedHmac = csrfCookie.split(".")[1];

  let verified = false;
  for (const key of process.env.CSRF_COOKIE_SIGNING_KEYS?.split(',') || []) {
    const hmac = crypto.createHmac("sha256", key);
    const messageToTest = message;
    const computedHmac = hmac.update(messageToTest).digest("hex").toString();

    const doMatch = crypto.timingSafeEqual(
      Buffer.from(computedHmac),
      Buffer.from(receivedHmac)
    );

    if (doMatch) {
      verified = true;
      break;
    }
  }

  if (!verified) {
    res.status(400).send("invalid CSRF signature");
    console.error("CSRF SIGNATURE ERROR");
    return;
  }

  const csrfSessionId = message.split("!")[0];

  if (csrfSessionId !== sessionId) {
    res.status(400).send("CSRF token does not match session ID");
    console.error("CSRF SESSION ID ERROR");
    return;
  }

  next();
};
