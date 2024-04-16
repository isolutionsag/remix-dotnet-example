import type { LoaderFunctionArgs, MetaFunction } from "@remix-run/node";
import { authenticator } from "~/services/auth.server";
export const meta: MetaFunction = () => {
  return [
    { title: "Cupcake Todo-List" },
    { name: "description", content: "Welcome to the best todo list ever!" },
  ];
};

export async function loader({ request }: LoaderFunctionArgs) {
  await authenticator.isAuthenticated(request, { successRedirect: "/lists" });
  return null;
}

export default function Index() {
  return <></>;
}
