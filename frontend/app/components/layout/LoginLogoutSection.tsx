import { useUserContext } from "../contexts/UserContext";
import { Button } from "@mui/material";
import { Form } from "@remix-run/react";
import { useTranslation } from "react-i18next";

export function LoginLogoutSection() {
  const user = useUserContext();
  const { t } = useTranslation();
  return (
    <div>
      {user != null ? (
        <Form method="post">
          <Button type="submit">
            {t("loginLogout:logout")} {user.displayname}
          </Button>
          {user.roles.includes("Admin") && (
            <div className="attentionText">{t("loginLogout:adminMode")}</div>
          )}
        </Form>
      ) : (
        <Button href="/login">{t("loginLogout:login")}</Button>
      )}
    </div>
  );
}

