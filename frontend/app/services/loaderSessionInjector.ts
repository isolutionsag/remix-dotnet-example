import { json } from "@remix-run/react";
import { getSession } from "./session.server";

class LoaderSessionInjector {
  private sessionValue: string | null = null;
  constructor() {}

  public setSessionValue(sessionValue: string): void {
    this.sessionValue = sessionValue;
  }

  public produceResponse<T extends object>(request: Request, data: T) {
    return json(data, {
      headers: [
        [
          "set-cookie",
          this.sessionValue ?? getSession(request.headers.get("Cookie")),
        ],
      ] as HeadersInit,
    });
  }
}

export const loaderSessionInjector = new LoaderSessionInjector();
