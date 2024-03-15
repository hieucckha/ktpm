import { useEffect, type FC, useContext } from "react";

import NavBarLogin from "../partials/NavBarLogin";
import Sidebar from "../partials/Sidebar";
import { Outlet } from "react-router-dom";
import connection from "../utils/Notification";
import notificationService from "../services/notification.service";
import localStorageService from "../services/localStorage.service";
import {
	NotificationContext,
	NotificationProvider,
} from "../context/NotificationContext";

/**
 * Home page.
 */

const AppLayout: FC = () => {
	const { setNotifications } = useContext(NotificationContext);

	useEffect(() => {
		connection.on("ReceiveNotification", (user, message) => {
			console.log(`All::${user}::Received notification::${message}`);
			const userEmail = localStorageService.getItem("user");
			if (userEmail === user) {
				console.log(`${user}::Received notification::${message}`);
				const parsedMessage = JSON.parse(message);
				notificationService.addNotification(parsedMessage as any);
				setNotifications(notificationService.getNotifications());
			}
		});

		if (connection.state === "Disconnected") {
			connection.start();
		}

		return () => {
			connection.off("ReceiveNotification");
			if (connection.state === "Connected") {
				connection.stop();
			}
		};
	}, []);

	return (
		// <div className="flex flex-col h-screen">
		<div className="bg-white">
			<NavBarLogin />
			<div className="bg-white w-full h-screen flex">
				<div className="flex-none sm:w-64 h-full ...">
					<Sidebar />
				</div>
				<div className="grow h-full ...">
					<Outlet />
				</div>
			</div>
		</div>
	);
};

export default AppLayout;
