import { useState } from "react";

export function NameLengthBad() {
  const [name, setName] = useState("");
  const [length, setLength] = useState(0);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setName(e.target.value);
    setLength(e.target.value.length);
  };

  return (
    <div>
      <input value={name} onChange={handleChange} />
      <p>Length: {length}</p>
    </div>
  );
}