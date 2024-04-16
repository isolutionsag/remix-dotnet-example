import { useTranslation } from "react-i18next";

export function LoginErrorText() {
  const { t } = useTranslation();
  return (
    <div>
      <p>{t("loginError:text")}</p>
    </div>
  );
}
