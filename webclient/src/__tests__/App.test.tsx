import { render } from "@testing-library/react";
import React from "react";
import Home from "../components/Home";

it("should render", () => {
  const { getByText } = render(<Home />);
  expect(
    getByText(/Front End Course Directory/i)
  ).toBeInTheDocument();
});
