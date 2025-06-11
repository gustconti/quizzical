import { useEffect } from "react";

export function Child({ value }: { value: number }) {
  useEffect(() => {
    console.log("Value changed:", value);
  }, [value]);

  return <p>Child received: {value}</p>;
}