import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  json,
  redirect,
} from "@remix-run/node";
import { useLoaderData, useSubmit } from "@remix-run/react";
import { TodoList, TodoListCategory } from "~/models/TodoList";
import { ApiClient } from "~/services/api/ApiClient.server";

import { useCallback, useEffect, useState } from "react";
import TodoListTable from "~/components/Tables/TodoListTable";
import { useTranslation } from "react-i18next";
import { Typography } from "@mui/material";
import { preventAnonymousAccess } from "~/auth/authHelper";

const NEW_LIST_ID = "";
const NEW_CATEGORY_ID = "";

export const handle = { i18n: ["general", "list"] };

export async function loader({ request }: LoaderFunctionArgs) {
  await preventAnonymousAccess(request);

  const apiClient = new ApiClient(request);
  const lists = await apiClient.get<TodoList[]>("TodoList");
  const categories =
    await apiClient.get<TodoListCategory[]>("TodoListCategory");

  return json({ lists: lists, categories: categories });
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
      ) as TodoList;
      entry.id === NEW_LIST_ID
        ? await apiClient.post<TodoList>("TodoList", JSON.stringify(entry))
        : await apiClient.put<TodoList>("TodoList", JSON.stringify(entry));
    } catch (e) {
      console.log("error on PUT - POST ", e);
    }
  } else if (request.method === "DELETE") {
    try {
      const deleteId = formData.get("delete_id");
      await apiClient.delete("TodoList/" + deleteId);
    } catch (e) {
      console.error(e);
    }
  }
  return redirect("");
}

export default function Lists() {
  const [lists, setLists] = useState<TodoList[]>([]);
  const [editingEntry, setEditingEntry] = useState<TodoList | undefined>(
    undefined
  );
  const submit = useSubmit();
  const { t } = useTranslation();

  const loaderData = useLoaderData<typeof loader>();
  useEffect(() => {
    if (
      JSON.stringify(lists) !== JSON.stringify(loaderData.lists) &&
      editingEntry?.id !== NEW_LIST_ID
    ) {
      setLists(loaderData.lists as TodoList[]);
    }
  }, [lists, loaderData.lists, editingEntry]);

  const categories = loaderData.categories as TodoListCategory[];

  const handleNameChange = useCallback(
    (value: string | null) => {
      const newEntry = { ...editingEntry, name: value ?? "" } as TodoList;
      setEditingEntry(newEntry);
    },
    [editingEntry]
  );

  const handleCategoryChange = useCallback(
    (value: string | null) => {
      const newEntry = { ...editingEntry, categoryId: value ?? "" } as TodoList;
      setEditingEntry(newEntry);
    },
    [editingEntry]
  );

  const handleEditClick = (entry: TodoList) => {
    setEditingEntry(entry);
  };

  const handleCreateClick = () => {
    const newEntry = {
      id: NEW_LIST_ID,
      name: "",
      categoryId: NEW_CATEGORY_ID,
      category: undefined,
      entries: [],
    } as TodoList;
    setLists([newEntry, ...lists]);
    setEditingEntry(newEntry);
  };

  const handleCancelClick = () => {
    setEditingEntry(undefined);
    if (editingEntry?.id === NEW_LIST_ID) {
      setLists(lists.filter((x) => x.id !== NEW_LIST_ID));
    }
  };

  const handleSaveClick = () => {
    const data = new FormData();
    data.append("save_entry", JSON.stringify(editingEntry));

    setEditingEntry(undefined);

    submit(data, { method: editingEntry?.id === NEW_LIST_ID ? "POST" : "PUT" });
  };

  const handleDeleteClick = (entry: TodoList) => {
    const data = new FormData();
    data.append("delete_id", entry.id);

    submit(data, { method: "DELETE" });
  };

  return (
    <>
      <Typography variant="h2">{t("list:title")}</Typography>

      <TodoListTable
        lists={lists}
        categories={categories}
        editingEntry={editingEntry}
        handleCreateClick={handleCreateClick}
        handleSaveClick={handleSaveClick}
        handleCancelClick={handleCancelClick}
        handleDeleteClick={handleDeleteClick}
        handleEditClick={handleEditClick}
        handleNameChange={handleNameChange}
        handleCategoryChange={handleCategoryChange}
      ></TodoListTable>
    </>
  );
}
