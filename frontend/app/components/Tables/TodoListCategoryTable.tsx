import { useTranslation } from "react-i18next";
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Button,
  TextField,
} from "@mui/material";
import { TodoListCategory } from "~/models/TodoList";
import { MouseEventHandler } from "react";

interface Props {
  categories: TodoListCategory[];
  editingEntry: TodoListCategory | undefined;
  handleCreateClick: MouseEventHandler<HTMLButtonElement>;
  handleSaveClick: MouseEventHandler<HTMLButtonElement>;
  handleCancelClick: MouseEventHandler<HTMLButtonElement>;
  handleDeleteClick: (entry: TodoListCategory) => void;
  handleEditClick: (entry: TodoListCategory) => void;
  handleNameChange: (value: string | null) => void;
}

export default function TodoListCategoryTable({
  categories,
  editingEntry,
  handleCreateClick,
  handleNameChange,
  handleSaveClick,
  handleCancelClick,
  handleDeleteClick,
  handleEditClick,
}: Props) {
  const { t } = useTranslation();

  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell colSpan={4}>
              <Button variant="contained" onClick={handleCreateClick}>
                {t("general:create")}
              </Button>
            </TableCell>
          </TableRow>
          <TableRow>
            <TableCell>{t("general:id")}</TableCell>
            <TableCell>{t("general:name")}</TableCell>
            <TableCell>{t("general:action")}</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {categories.map((entry) => (
            <TableRow key={entry.id}>
              <TableCell>{entry.id}</TableCell>
              {/* edit mode */}
              {editingEntry?.id === entry.id ? (
                <>
                  <TableCell>
                    <TextField
                      variant="standard"
                      onChange={(event) =>
                        handleNameChange(event?.target.value)
                      }
                      placeholder={t("list:default_name")}
                      value={editingEntry.name}
                    />
                  </TableCell>

                  <TableCell>
                    <Button onClick={handleSaveClick} variant="contained">
                      {t("general:save")}
                    </Button>
                    <Button variant="contained" onClick={handleCancelClick}>
                      {t("general:cancel")}
                    </Button>
                  </TableCell>
                </>
              ) : (
                <>
                  <TableCell>
                    <>{entry.name}</>
                  </TableCell>

                  <TableCell>
                    <Button
                      variant="outlined"
                      onClick={() => handleEditClick(entry)}
                    >
                      {t("general:edit")}
                    </Button>
                    <Button
                      onClick={() => handleDeleteClick(entry)}
                      variant="outlined"
                    >
                      {t("general:delete")}
                    </Button>
                  </TableCell>
                </>
              )}
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
