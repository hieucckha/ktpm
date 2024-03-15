/** @type {import('tailwindcss').Config} */
export default {
	content: [
		"./src/**/*.{js,jsx,ts,tsx}",
		"./node_modules/flowbite/**/*.js",
		"node_modules/flowbite-react/lib/esm/**/*.js",
	],
	theme: {
		extend: {
			spacing: {
				"screen-dvh": "100dvh",
			},
		},
	},
	plugins: [require("daisyui"), require("flowbite/plugin")],
};
