import React from "react";
import {
	BookOutlined,
	ControlOutlined,
	HomeOutlined,
	UsergroupAddOutlined,
} from "@ant-design/icons";
import type { MenuProps } from "antd";
import { Menu, Skeleton } from "antd";
import { NavLink, useLocation } from "react-router-dom";
import { useGetAllClassesQuery } from "../api/store/class/queries";
import useAuth from "../hooks/auth";
import { UserRole } from "../api/store/auth/interface";

type MenuItem = Required<MenuProps>["items"][number];

function getItem(
	label: React.ReactNode,
	key: React.Key,
	icon?: React.ReactNode,
	children?: MenuItem[],
	type?: "group"
): MenuItem {
	return {
		key,
		icon,
		children,
		label,
		type,
	} as MenuItem;
}

const fontSizeIcon = "20px";
const fontSizeMenu = "16px";

const Sidebar: React.FC = () => {
	const { data: user, isLoading: isProfileLoading } = useAuth();
	const { data, isLoading } = useGetAllClassesQuery(user?.id);
	const { pathname } = useLocation();

	const getSelectedKey = (pathname: string): string[] => {
		const splitPath = pathname.split("/").filter((item) => item !== "");
		if (splitPath.length === 1) {
			return splitPath;
		}

		return [splitPath.slice(0, 2).join("/")];
	};

	const getDefaultOpenClass = (pathname: string): string[] => {
		const splitPath = pathname.split("/").filter((item) => item !== "");
		if (splitPath.includes("admin")) {
			return [];
		}

		if (splitPath.includes("class")) {
			return ["classes"];
		}

		return [];
	};

	const menuBuilder = (): MenuProps["items"] => {
		let userMenu = [
			getItem(
				<NavLink style={{ fontSize: fontSizeMenu }} to={"home"}>
					Home
				</NavLink>,
				"home",
				<HomeOutlined style={{ fontSize: fontSizeIcon }} />
			),
		];

		if (isLoading || isProfileLoading) {
			userMenu.push(
				getItem(
					<div className="flex flex-row items-center gap-x-2">
						<Skeleton.Input className="w-14 h-6" active={true} size={"small"} />
					</div>,
					"skeleton",
					<Skeleton.Avatar
						className="w-14 h-8"
						active={true}
						size={"default"}
						shape={"circle"}
					/>
				)
			);
			return userMenu;
		}

		if (user?.role === UserRole.Admin) {
			userMenu = [
				getItem(
					<NavLink style={{ fontSize: fontSizeMenu }} to={"/admin/classes"}>
						Management class
					</NavLink>,
					"admin/classes",
					<ControlOutlined style={{ fontSize: fontSizeIcon }} />
				),
				getItem(
					<NavLink style={{ fontSize: fontSizeMenu }} to={"/admin/users"}>
						Management user
					</NavLink>,
					"admin/users",
					<UsergroupAddOutlined style={{ fontSize: fontSizeIcon }} />
				),
			];
			return userMenu;
		}

		userMenu.push(
			getItem(
				"Class",
				"classes",
				<BookOutlined style={{ fontSize: fontSizeIcon }} />,
				data?.map((item) =>
					getItem(
						<NavLink to={`class/${item.id}/overview`}>{item.name}</NavLink>,
						`class/${item.id}`
					)
				)
			)
		);
		return userMenu;
	};

	return (
		<div
			className="fixed top-0 left-0 z-40 w-64 h-screen pt-16 transition-transform -translate-x-full bg-white border-r border-gray-200 sm:translate-x-0 dark:bg-gray-800 dark:border-gray-700"
			style={{ width: 256 }}
		>
			<Menu
				className="mt-0"
				defaultSelectedKeys={getSelectedKey(pathname)}
				selectedKeys={getSelectedKey(pathname)}
				defaultOpenKeys={getDefaultOpenClass(pathname)}
				mode="inline"
				theme="light"
				inlineCollapsed={false}
				items={menuBuilder()}
			/>
		</div>
	);
};

export default Sidebar;
