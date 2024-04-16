import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  json,
  redirect,
} from "@remix-run/node";
import { useLoaderData, useSubmit } from "@remix-run/react";
import { TodoList, TodoListCategory } from "~/models/TodoList";
import { ApiClient } from "~/services/api/ApiClient.server";
import { Typography } from "@mui/material";

import { useEffect, useState } from "react";
import TodoListCategoryTable from "~/components/Tables/TodoListCategoryTable";
import { useTranslation } from "react-i18next";
import { preventAnonymousAccess } from "~/auth/authHelper";

const NEW_CATEGORY_ID = "";

export const handle = { i18n: ["general", "category"] };

export async function loader({ request }: LoaderFunctionArgs) {
  await preventAnonymousAccess(request);

  const apiClient = new ApiClient(request);
  const categories =
    await apiClient.get<TodoListCategory[]>("TodoListCategory");

  return json({ categories: categories });
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
      entry.id === NEW_CATEGORY_ID
        ? await apiClient.post<TodoListCategory>(
            "TodoListCategory",
            JSON.stringify(entry)
          )
        : await apiClient.put<TodoListCategory>(
            "TodoListCategory",
            JSON.stringify(entry)
          );
    } catch (e) {
      console.log("error on PUT - POST ", e);
    }
  } else if (request.method === "DELETE") {
    try {
      const deleteId = formData.get("delete_id");
      await apiClient.delete("TodoListCategory/" + deleteId);
    } catch (e) {
      console.error(e);
    }
  }
  return redirect("");
}

export default function Categories() {
  const [categories, setCategories] = useState<TodoListCategory[]>([]);
  const [editingEntry, setEditingEntry] = useState<
    TodoListCategory | undefined
  >(undefined);

  const submit = useSubmit();
  const { t } = useTranslation();
  const loaderData = useLoaderData<typeof loader>();

  // Update categories when loader data changes
  useEffect(() => {
    if (
      JSON.stringify(categories) !== JSON.stringify(loaderData.categories) &&
      editingEntry?.id !== NEW_CATEGORY_ID
    ) {
      setCategories(loaderData.categories as TodoListCategory[]);
    }
  }, [categories, loaderData.categories, editingEntry]);

  // Update handlers
  const handleNameChange = (value: string | null) => {
    const newEntry = { ...editingEntry, name: value ?? "" } as TodoListCategory;
    setEditingEntry(newEntry);
  };

  const handleEditClick = (entry: TodoListCategory) => {
    setEditingEntry(entry);
  };

  const handleCreateClick = () => {
    const newEntry = { id: NEW_CATEGORY_ID, name: "" } as TodoListCategory;
    setCategories([newEntry, ...categories]);
    setEditingEntry(newEntry);
  };

  const handleCancelClick = () => {
    setEditingEntry(undefined);
    if (editingEntry?.id === NEW_CATEGORY_ID) {
      setCategories(categories.filter((x) => x.id !== NEW_CATEGORY_ID));
    }
  };

  const handleSaveClick = () => {
    const data = new FormData();
    data.append("save_entry", JSON.stringify(editingEntry));

    setEditingEntry(undefined);

    submit(data, {
      method: editingEntry?.id === NEW_CATEGORY_ID ? "POST" : "PUT",
    });
  };

  const handleDeleteClick = (entry: TodoListCategory) => {
    const data = new FormData();
    data.append("delete_id", entry.id);

    submit(data, { method: "DELETE" });
  };

  return (
    <>
      <Typography variant="h2">{t("category:title")}</Typography>
      <TodoListCategoryTable
        categories={categories}
        editingEntry={editingEntry}
        handleCreateClick={handleCreateClick}
        handleSaveClick={handleSaveClick}
        handleCancelClick={handleCancelClick}
        handleDeleteClick={handleDeleteClick}
        handleEditClick={handleEditClick}
        handleNameChange={handleNameChange}
      ></TodoListCategoryTable>
    </>
  );
}
