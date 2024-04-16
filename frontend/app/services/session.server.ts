import { Cookie, CookieOptions, createSessionStorage } from "@remix-run/node";
import { getClient } from "~/redisClient.server";

export const SESSION_COOKIE_NAME = "__session";

function createDatabaseSessionStorage({
  cookie: cookieOptions,
}: {
  cookie: Cookie | CookieOptions;
}) {
  // Configure your database client...
  return createSessionStorage({
    cookie: cookieOptions,
    async createData(data) {
      const redisClient = await getClient();
      redisClient.connect();

      const id = crypto.randomUUID();
      await redisClient.set(id, JSON.stringify(data));
      redisClient.disconnect();
      return id;
    },
    async readData(id) {
      const redisClient = await getClient();
      redisClient.connect();
      const result = JSON.parse((await redisClient.get(id)) || "{}");
      redisClient.disconnect();
      return result;
    },
    async updateData(id, data) {
      const redisClient = await getClient();
      redisClient.connect();
      await redisClient.set(id, JSON.stringify(data));
      redisClient.disconnect();
    },
    async deleteData(id) {
      const redisClient = await getClient();
      redisClient.connect();
      await redisClient.del(id);
      redisClient.disconnect();
    },
  });
}

export const sessionStorage = createDatabaseSessionStorage({
  cookie: {
    name: SESSION_COOKIE_NAME,
    sameSite: "lax", 
    path: "/", //  will work in all routes
    httpOnly: true, 
    secrets: process.env.SESSION_COOKIE_SIGNING_KEYS?.split(',') || [],
    secure: true,
    maxAge: 60 * 60 * 24 * 30,
  },
});
export const { getSession, commitSession, destroySession } = sessionStorage;
