import { Select, MenuItem } from "@mui/material";

interface SelectionProps<T> {
  items: T[];
  currentId: string;
  handleChange: (newId: string | null) => void;
  getKey: (item: T) => string;
  getValue: (item: T) => string;
}

export default function MySelection<T>({
  items,
  currentId,
  handleChange,
  getKey,
  getValue,
}: SelectionProps<T>) {
  return (
    <Select
      id="select-category"
      value={currentId}
      onChange={(event) => handleChange(event?.target.value)}
    >
      {items.map((item) => (
        <MenuItem value={getKey(item)} key={getKey(item)}>
          {""}
          {getValue(item)}
        </MenuItem>
      ))}
    </Select>
  );
}
