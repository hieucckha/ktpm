import ReactDOM from "react-dom/client";
import { RouterProvider } from "react-router-dom";

import "flowbite";
import router from "./router";
import "./styles/tailwind.css";
import "./styles/index.scss";
import React from "react";

const rootElement: HTMLElement | null = document.querySelector("#root");
if (rootElement == null) {
	throw new Error("Failed to find root element");
}

ReactDOM.createRoot(rootElement).render(
	<React.StrictMode>
		<RouterProvider router={router} />
	</React.StrictMode>
);
