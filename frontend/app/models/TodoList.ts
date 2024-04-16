export interface TodoList {
  id: string;
  name: string;
  categoryId: string;
  category: TodoListCategory | undefined;
  entries: TodoListEntry[];
}

export interface TodoListEntry {
  id: string;
  parentId: string;
  title: string;
  description: string;
  isCompleted: boolean;
}

export interface TodoListCategory {
  id: string;
  name: string;
}

export interface InputError<T> {
  field: string;
  element: T;
}
