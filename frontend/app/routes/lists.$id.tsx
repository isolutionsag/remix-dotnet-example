import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  json,
  redirect,
} from "@remix-run/node";
import { useLoaderData, useSubmit } from "@remix-run/react";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import TodoListEntryTable from "~/components/Tables/TodoListEntryTable";
import { TodoList, TodoListEntry } from "~/models/TodoList";
import { Typography } from "@mui/material";
import { ApiClient } from "~/services/api/ApiClient.server";
import { preventAnonymousAccess } from "~/auth/authHelper";

const NEW_LISTENTRY_ID = "";

export const handle = { i18n: ["general", "listentries"] };

export async function loader({ request, params }: LoaderFunctionArgs) {
  await preventAnonymousAccess(request);
  const apiClient = new ApiClient(request);
  const entries = await apiClient.get<TodoListEntry[]>(
    "TodoListEntry/list/" + params.id
  );
  const list = await apiClient.get<TodoList>("TodoList/" + params.id);

  return json({ entries, list });
}

export async function action({ request }: ActionFunctionArgs) {
  await preventAnonymousAccess(request);
  const apiClient = new ApiClient(request);
  const formData = await request.formData();

  // POST, PUT to api
  if (request.method === "PUT" || request.method === "POST") {
    try {
      const entry = JSON.parse(
        formData.get("save_entry")!.toString()
      ) as TodoListEntry;
      entry.id === NEW_LISTENTRY_ID
        ? await apiClient.post<TodoListEntry>(
            "TodoListEntry",
            JSON.stringify(entry)
          )
        : await apiClient.put<TodoListEntry>(
            "TodoListEntry",
            JSON.stringify(entry)
          );
    } catch (e) {
      console.log("error on PUT - POST ", e);
    }
  } else if (request.method === "DELETE") {
    try {
      const deleteId = formData.get("delete_id");
      await apiClient.delete("TodoListEntry/" + deleteId);
    } catch (e) {
      console.error(e);
    }
  }
  return redirect("");
}

export default function Lists() {
  const [entries, setEntries] = useState<TodoListEntry[]>([]);
  const [editingEntry, setEditingEntry] = useState<TodoListEntry | undefined>(
    undefined
  );
  const submit = useSubmit();
  const { t } = useTranslation();

  // update react state from remix loader
  const loaderData = useLoaderData<typeof loader>();
  const currentList = loaderData.list;
  useEffect(() => {
    if (
      JSON.stringify(entries) !== JSON.stringify(loaderData.entries) &&
      editingEntry?.id !== NEW_LISTENTRY_ID
    ) {
      setEntries(loaderData.entries as TodoListEntry[]);
    }
  }, [entries, loaderData.entries, editingEntry]);

  const handleTitleChange = (value: string | null) => {
    const newEntry = { ...editingEntry, title: value ?? "" } as TodoListEntry;
    setEditingEntry(newEntry);
  };

  const handleDescriptionChange = (value: string | null) => {
    const newEntry = {
      ...editingEntry,
      description: value ?? "",
    } as TodoListEntry;
    setEditingEntry(newEntry);
  };

  const handleCompletedChange = (value: boolean) => {
    const newEntry = { ...editingEntry, isCompleted: value } as TodoListEntry;
    setEditingEntry(newEntry);
  };

  const handleEditClick = (entry: TodoListEntry) => {
    setEditingEntry(entry);
  };

  const handleCreateClick = () => {
    const newEntry = {
      id: NEW_LISTENTRY_ID,
      title: "",
      description: "",
      parentId: currentList?.id,
      isCompleted: false,
    } as TodoListEntry;
    setEntries([newEntry, ...entries]);
    setEditingEntry(newEntry);
  };

  const handleCancelClick = () => {
    setEditingEntry(undefined);
    if (editingEntry?.id === NEW_LISTENTRY_ID) {
      setEntries(entries.filter((x) => x.id !== NEW_LISTENTRY_ID));
    }
  };

  const handleSaveClick = () => {
    const data = new FormData();
    data.append("save_entry", JSON.stringify(editingEntry));
    setEditingEntry(undefined);

    submit(data, {
      method: editingEntry?.id === NEW_LISTENTRY_ID ? "POST" : "PUT",
    });
  };

  const handleDeleteClick = (entry: TodoListEntry) => {
    const data = new FormData();
    data.append("delete_id", entry.id);

    submit(data, { method: "DELETE" });
  };

  return (
    <>
      <Typography variant="h2">
        {t("listentries:title")} {currentList?.name}
      </Typography>

      <TodoListEntryTable
        entries={entries}
        editingEntry={editingEntry}
        handleCreateClick={handleCreateClick}
        handleSaveClick={handleSaveClick}
        handleCancelClick={handleCancelClick}
        handleDeleteClick={handleDeleteClick}
        handleEditClick={handleEditClick}
        handleTitleChange={handleTitleChange}
        handleDescriptionChange={handleDescriptionChange}
        handleCompletedChange={handleCompletedChange}
      />
    </>
  );
}
