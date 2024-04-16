import {
  AUTH_CLIENT_ID,
  AUTH_TENANT_ID,
  AUTH_CLIENT_SCOPE,
  AUTH_CLIENT_SECRET,
  REFRESH_TOKEN_EXPIRES_IN,
  authenticator,
} from "../auth.server";
import fetch from "node-fetch";
import { json } from "@remix-run/node";
import type { Response } from "node-fetch";
import { UserWithTokens } from "~/models/User";
import { commitSession, getSession } from "../session.server";
import { jwtDecode } from "jwt-decode";

export class ApiClient {
  private user: UserWithTokens | null = null;
  private baseUrl = "https://localhost:7156/";

  private async updateUserInSession(): Promise<unknown> {
    const session = await getSession(this.request.headers.get("cookie"));
    const user = session.data.user as UserWithTokens;
    const isAccessTokenValid =
      new Date(Date.now()) < new Date(user.tokens.access.expiresAt);
    const isRefreshTokenValid =
      new Date(Date.now()) < new Date(user.tokens.refresh.expiresAt);
    if (!isRefreshTokenValid) {
      await authenticator.logout(this.request, { redirectTo: "login" });
    }
    if (!isAccessTokenValid) {
      console.debug("Refreshing access token");
      const tokenUrl = `https://login.microsoftonline.com/${AUTH_TENANT_ID}/oauth2/v2.0/token`;
      const params = new URLSearchParams({
        grant_type: "refresh_token",
        client_id: AUTH_CLIENT_ID,
        client_secret: AUTH_CLIENT_SECRET,
        scope: AUTH_CLIENT_SCOPE,
        redirectUri: "https://localhost:3000/auth/microsoft/callback",
        refresh_token: user.tokens.refresh.token,
      });
      const response = await fetch(tokenUrl, {
        method: "POST",
        body: params.toString(),
        headers: {
          "Content-Type": "application/x-www-form-urlencoded",
        },
      });
      if (response.status === 200) {
        console.debug("Successfully refreshed access token");
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const newTokenResponse = (await response.json()) as any;
        const newAccessToken = newTokenResponse.access_token;
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const newAccessTokenDecoded = jwtDecode(newAccessToken) as any;
        const newAccessTokenExpiresAt = new Date(
          Date.now() + newTokenResponse.expires_in * 1000
        ).toISOString();
        const newRefreshToken = newTokenResponse.refresh_token;
        const newRefreshTokenExpiresAt = new Date(
          Date.now() + REFRESH_TOKEN_EXPIRES_IN * 1000
        ).toISOString();
        user.tokens.access.token = newAccessToken;
        user.tokens.access.expiresAt = newAccessTokenExpiresAt;
        user.tokens.refresh.token = newRefreshToken;
        user.tokens.refresh.expiresAt = newRefreshTokenExpiresAt;
        user.roles = newAccessTokenDecoded.roles;

        session.data.user = user;
      }
      commitSession(session);
    }
    this.user = user;

    return;
  }

  constructor(private request: Request) {
    if (process.env.NODE_ENV === "development") {
      process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    }

    authenticator.isAuthenticated(request).then((user) => {
      if (user != null) {
        this.updateUserInSession();
      }
    });
  }

  public async get<TResponse extends object>(
    url: string
  ): Promise<TResponse | undefined> {
    await this.updateUserInSession();
    const response = await fetch(`${this.baseUrl}${url}`, {
      method: "get",
      headers: {
        Authorization: `Bearer ${this.user?.tokens.access.token}`,
      },
    });
    if (response.ok) {
      return this.getSuccessData<TResponse>(response);
    } else {
      throw this.getFailResponse(response.status);
    }
  }

  public async post<TResponse extends object>(
    url: string,
    data: string
  ): Promise<TResponse | undefined> {
    await this.updateUserInSession();
    console.debug("Data for post: ", data);
    const response = await fetch(`${this.baseUrl}${url}`, {
      method: "post",
      body: data,
      headers: {
        Authorization: `Bearer ${this.user?.tokens.access.token}`,
        "Content-Type": "application/json; charset=utf-8",
      },
    });
    if (response.ok) {
      return this.getSuccessData<TResponse>(response);
    } else {
      throw this.getFailResponse(response.status);
    }
  }

  public async put<TResponse extends object>(
    url: string,
    data: string
  ): Promise<TResponse | undefined> {
    await this.updateUserInSession();
    const response = await fetch(`${this.baseUrl}${url}`, {
      method: "put",
      body: data,
      headers: {
        Authorization: `Bearer ${this.user?.tokens.access.token}`,
        "Content-Type": "application/json; charset=utf-8",
      },
    });
    if (response.ok) {
      return this.getSuccessData<TResponse>(response);
    } else {
      throw this.getFailResponse(response.status);
    }
  }

  public async delete<TResponse extends object>(
    url: string
  ): Promise<TResponse | undefined> {
    await this.updateUserInSession();
    const response = await fetch(`${this.baseUrl}${url}`, {
      method: "delete",
      headers: {
        Authorization: `Bearer ${this.user?.tokens.access.token}`,
      },
    });
    if (response.ok) {
      return this.getSuccessData<TResponse>(response);
    } else {
      throw this.getFailResponse(response.status);
    }
  }

  private getFailResponse(status: number = 500) {
    return json(null, {
      status,
    });
  }

  private async getSuccessData<TResponse extends object>(response: Response) {
    const contentType = response.headers.get("content-type");
    let responseData: TResponse = {} as TResponse;
    if (contentType && contentType.indexOf("application/json") !== -1) {
      responseData = (await response.json()) as TResponse;
    }
    return responseData;
  }
}
