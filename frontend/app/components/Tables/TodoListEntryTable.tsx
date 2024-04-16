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
  Switch,
} from "@mui/material";
import { TodoListEntry } from "~/models/TodoList";
import { MouseEventHandler } from "react";

interface Props {
  entries: TodoListEntry[];
  editingEntry: TodoListEntry | undefined;
  handleCreateClick: MouseEventHandler<HTMLButtonElement>;
  handleSaveClick: MouseEventHandler<HTMLButtonElement>;
  handleCancelClick: MouseEventHandler<HTMLButtonElement>;
  handleDeleteClick: (entry: TodoListEntry) => void;
  handleEditClick: (entry: TodoListEntry) => void;
  handleTitleChange: (value: string | null) => void;
  handleDescriptionChange: (value: string | null) => void;
  handleCompletedChange: (value: boolean) => void;
}

export default function TodoListEntryTable({
  entries,
  editingEntry,
  handleTitleChange,
  handleDescriptionChange,
  handleCompletedChange,
  handleCreateClick,
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
            <TableCell colSpan={5}>
              <Button variant="contained" onClick={handleCreateClick}>
                {t("general:create")}
              </Button>
            </TableCell>
          </TableRow>
          <TableRow>
            <TableCell>{t("general:id")}</TableCell>
            <TableCell>{t("general:name")}</TableCell>
            <TableCell>{t("listentries:description")}</TableCell>
            <TableCell>{t("listentries:isCompleted")}</TableCell>
            <TableCell>{t("general:action")}</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {entries.map((entry) => (
            <TableRow key={entry.id}>
              <TableCell>{entry.id}</TableCell>
              {/* edit mode */}
              {editingEntry?.id === entry.id ? (
                <>
                  <TableCell>
                    <TextField
                      variant="standard"
                      onChange={(event) =>
                        handleTitleChange(event?.target.value)
                      }
                      placeholder={t("listentries:default_title")}
                      value={editingEntry.title}
                    />
                  </TableCell>

                  <TableCell>
                    <TextField
                      variant="standard"
                      multiline
                      maxRows={4}
                      onChange={(event) =>
                        handleDescriptionChange(event?.target.value)
                      }
                      placeholder={t("listentries:default_description")}
                      value={editingEntry.description}
                    />
                  </TableCell>

                  <TableCell>
                    <Switch
                      checked={editingEntry.isCompleted}
                      onChange={(_, checked) => handleCompletedChange(checked)}
                      inputProps={{ "aria-label": "controlled" }}
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
                    <>{entry.title}</>
                  </TableCell>

                  <TableCell>{entry.description}</TableCell>

                  <TableCell>
                    <Switch checked={entry.isCompleted} disabled />
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
