import { authenticator } from "~/services/auth.server";

export async function preventAnonymousAccess(request: Request) {
	await authenticator.isAuthenticated(request, {
		failureRedirect: "/",
	});
}