import { useEffect, type FC } from "react";
import { Outlet } from "react-router-dom";
import AOS from "aos";
import "aos/dist/aos.css";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { GoogleOAuthProvider } from "@react-oauth/google";
import { FacebookProvider } from "react-facebook";
import { App as AntdApp } from "antd";
import { NotificationProvider } from "./context/NotificationContext";
const queryClient = new QueryClient();

/**
 * App.
 * @returns App.
 */
export const App: FC = () => {
	useEffect(() => {
		AOS.init({
			once: true,
			disable: "phone",
			duration: 700,
			easing: "ease-out-cubic",
		});
	});

	return (
		<GoogleOAuthProvider clientId="856018113300-i5i4padrol9e1nocd5ibvm2k1uuh70rm.apps.googleusercontent.com">
			<FacebookProvider appId="326344773641983">
				<QueryClientProvider client={queryClient}>
					<AntdApp>
						<NotificationProvider>
							<Outlet />
						</NotificationProvider>

						<ReactQueryDevtools initialIsOpen={false} />
					</AntdApp>
				</QueryClientProvider>
			</FacebookProvider>
		</GoogleOAuthProvider>
	);
};
