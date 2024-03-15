import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
	.withUrl(`${import.meta.env["VITE_API_URL"]}/notification`, {
		skipNegotiation: true,
		transport: signalR.HttpTransportType.WebSockets,
	})
	.build();

export default connection;
