/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { Authenticator } from "remix-auth";
import { MicrosoftStrategy } from "remix-auth-microsoft";
import { UserWithTokens } from "~/models/User";
import { sessionStorage } from "~/services/session.server";
import { jwtDecode } from "jwt-decode";

export const authenticator = new Authenticator<UserWithTokens>(sessionStorage);

export const AUTH_CLIENT_ID = process.env.AUTH_CLIENT_ID ?? "";
export const AUTH_CLIENT_SECRET = process.env.AUTH_CLIENT_SECRET ?? "";
export const AUTH_TENANT_ID = process.env.AUTH_TENANT_ID ?? "";
export const AUTH_CLIENT_SCOPE = process.env.AUTH_CLIENT_SCOPE ?? "";
export const REFRESH_TOKEN_EXPIRES_IN = (Number)(process.env.REFRESH_TOKEN_EXPIRES_IN ?? 86400);

authenticator.use(
  new MicrosoftStrategy(
    {
      clientId: AUTH_CLIENT_ID,
      clientSecret: AUTH_CLIENT_SECRET,
      redirectUri: "https://localhost:3000/auth/microsoft/callback",
      tenantId: AUTH_TENANT_ID,
      scope: AUTH_CLIENT_SCOPE,
      prompt: "login",
    },
    async ({ accessToken, refreshToken, extraParams, profile, request }) => {
      const accessTokenContent = jwtDecode(accessToken) as any;
      const idTokenContent = jwtDecode(extraParams.id_token) as any;
      const user: UserWithTokens = {
        displayname: idTokenContent.name,
        firstname: idTokenContent.firstname,
        lastname: idTokenContent.lastname,
        username: idTokenContent.preferred_username,
        roles: accessTokenContent?.roles ?? [],
        tokens: {
          access: {
            expiresAt: new Date(
              Date.now() + extraParams.expires_in * 1000
            ).toISOString(),
            token: accessToken,
          },
          refresh: {
            expiresAt: new Date(
              Date.now() + REFRESH_TOKEN_EXPIRES_IN * 1000
            ).toISOString(),
            token: refreshToken ?? "",
          },
        },
      };
      return user;
    }
  ),
  "microsoft"
);
