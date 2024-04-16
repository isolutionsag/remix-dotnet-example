import { AppBar, Toolbar, Typography, IconButton } from "@mui/material";
import { Container } from "@mui/system";
import { useNavigate } from "@remix-run/react";
import { useTranslation } from "react-i18next";
import { useUserContext } from "../contexts/UserContext";

const NavigationHeader = () => {
  const navigate = useNavigate();
  const { t } = useTranslation();
  const user = useUserContext();
  return (
    <AppBar position="static">
      <Container>
        <Toolbar>
          <Typography variant="h4">ToDo</Typography>
          {user != null ? (
            <>
              <IconButton onClick={() => navigate("lists")} color="inherit">
                <Typography variant="h6" component="div" sx={{ ml: 1 }}>
                  {t("layout:list")}
                </Typography>
              </IconButton>
              {user.roles.includes("Admin") ? (
                <IconButton
                  onClick={() => navigate("categories")}
                  color="inherit"
                >
                  {" "}
                  <Typography variant="h6" component="div" sx={{ ml: 1 }}>
                    {t("layout:categories")}
                  </Typography>
                </IconButton>
              ) : (
                <></>
              )}
              <IconButton onClick={() => navigate("form")} color="inherit">
                <Typography variant="h6" component="div" sx={{ ml: 1 }}>
                  {t("layout:form")}
                </Typography>
              </IconButton>
            </>
          ) : (
            <></>
          )}
        </Toolbar>
      </Container>
    </AppBar>
  );
};

export default NavigationHeader;
