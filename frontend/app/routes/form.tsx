import { Button } from "@mui/base";
import { ActionFunctionArgs } from "@remix-run/node";
import { Form, useActionData } from "@remix-run/react";
import { preventAnonymousAccess } from "~/auth/authHelper";

export async function action({ request }: ActionFunctionArgs) {
  await preventAnonymousAccess(request);
  // This action simply returns a success message.
  // You can extend it to handle your form data as needed.
  return { success: "Form submitted successfully!" };
}

export async function loader({ request }: ActionFunctionArgs) {
  await preventAnonymousAccess(request);
  return null;
}

export default function FormRoute() {
  // Optionally use actionData to show a message after submission
  const actionData = useActionData<typeof action>();

  return (
    <div>
      <h1>Simple Form</h1>
      {actionData && <p>{actionData.success}</p>}
      <Form method="post">
        {/* The form is empty as per the instructions, but you can add input fields here */}
        <Button type="submit">Submit</Button>
      </Form>
    </div>
  );
}
