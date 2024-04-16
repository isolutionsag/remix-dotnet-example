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
import { TodoList, TodoListCategory } from "~/models/TodoList";
import { useNavigate } from "@remix-run/react";
import { MouseEventHandler } from "react";
import MySelection from "../layout/MySelection";

interface Props {
  lists: TodoList[];
  categories: TodoListCategory[];
  editingEntry: TodoList | undefined;
  handleCreateClick: MouseEventHandler<HTMLButtonElement>;
  handleSaveClick: MouseEventHandler<HTMLButtonElement>;
  handleCancelClick: MouseEventHandler<HTMLButtonElement>;
  handleDeleteClick: (entry: TodoList) => void;
  handleEditClick: (entry: TodoList) => void;
  handleNameChange: (value: string | null) => void;
  handleCategoryChange: (value: string | null) => void;
}

export default function TodoListTable({
  lists,
  categories,
  editingEntry,
  handleCreateClick,
  handleNameChange,
  handleCategoryChange,
  handleSaveClick,
  handleCancelClick,
  handleDeleteClick,
  handleEditClick,
}: Props) {
  const { t } = useTranslation();
  const navigate = useNavigate();

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
            <TableCell>{t("list:category")}</TableCell>
            <TableCell>{t("general:action")}</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {lists.map((entry) => (
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
                      name="_newName"
                    />
                  </TableCell>

                  <TableCell>
                    <MySelection
                      items={categories}
                      currentId={editingEntry.categoryId}
                      handleChange={handleCategoryChange}
                      getKey={(item) => item.id}
                      getValue={(item) => item.name}
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

                  <TableCell>{entry.category?.name}</TableCell>

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
                    <Button
                      variant="outlined"
                      onClick={() =>
                        navigate(entry.id, { state: { name: entry.name } })
                      }
                    >
                      {t("list:entries")}
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
