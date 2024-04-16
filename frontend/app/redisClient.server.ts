import { createClient } from "redis";
export const getClient = async () => {
  const client = await createClient({
    url: process.env.REDIS_URL,
    password: process.env.REDIS_PASSWORD,
  });
  client.on("error", (error) => {
    console.error(error);
  });
  return client;
};
