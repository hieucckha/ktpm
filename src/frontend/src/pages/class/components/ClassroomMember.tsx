import { useState, type FC } from "react";
import { UserProfileDto } from "../../../api/store/auth/interface";
import ClassMember from "./ClassMember";
import useClassDetail from "../../../hooks/useClassDetail";
import InviteMemberByEmail from "../../../modal/InviteMemberByEmail";
import { Tooltip } from "antd";

interface MemberCardProps extends Partial<UserProfileDto> {}
const ClassroomMember: FC<MemberCardProps> = ({}) => {
	const { data: classDetail } = useClassDetail();
	const [isOpenInviteMemberByEmail, setIsOpenInviteMemberByEmail] =
		useState(false);
	const handleCloseModalInviteClass = (): void => {
		setIsOpenInviteMemberByEmail(false);
	};
	const handleOpenModalInviteClass = (): void => {
		setIsOpenInviteMemberByEmail(true);
	};

	return (
		<div className="w-3/5 flex flex-col items-center justify-start py-6">
			<div className="w-full">
				<div className=" pl-4 py-2 flex flex-row text-[rgb(25,103,210)] border-b border-b-[rgb(25,103,210)]">
					<div className="grow text-3xl align-middle">Teachers</div>
					<Tooltip placement="bottom" title="Add teacher" arrow>
						<svg
							onClick={handleOpenModalInviteClass}
							xmlns="http://www.w3.org/2000/svg"
							className="cursor-pointer"
							width={24}
							height={24}
							viewBox="0 0 24 24"
							fill="none"
							stroke="#000000"
							strokeWidth={2}
							strokeLinecap="round"
							strokeLinejoin="round"
						>
							<path d="M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
							<circle cx="8.5" cy={7} r={4} />
							<line x1={20} y1={8} x2={20} y2={14} />
							<line x1={23} y1={11} x2={17} y2={11} />
						</svg>
					</Tooltip>
				</div>

				{classDetail &&
					classDetail?.teachers?.map((item, idx) => (
						<ClassMember
							key={idx}
							className={"text-base px-4 w-full h-[52px] md:h-[64px]"}
							{...item}
						/>
					))}
			</div>
			<div className="w-full">
				<div className=" pl-4 py-2 flex flex-row text-[rgb(25,103,210)] border-b border-b-[rgb(25,103,210)]">
					<div className="grow text-3xl align-middle">Students</div>
					<Tooltip placement="bottom" title="Add student" arrow>
						<svg
							onClick={handleOpenModalInviteClass}
							xmlns="http://www.w3.org/2000/svg"
							className="cursor-pointer"
							width={24}
							height={24}
							viewBox="0 0 24 24"
							fill="none"
							stroke="#000000"
							strokeWidth={2}
							strokeLinecap="round"
							strokeLinejoin="round"
						>
							<path d="M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
							<circle cx="8.5" cy={7} r={4} />
							<line x1={20} y1={8} x2={20} y2={14} />
							<line x1={23} y1={11} x2={17} y2={11} />
						</svg>
					</Tooltip>
				</div>

				{classDetail &&
					classDetail?.students?.map((item, idx) => (
						<ClassMember
							key={idx}
							className={"text-base px-4 w-full h-[52px] md:h-[64px]"}
							{...item}
						/>
					))}
			</div>
			{isOpenInviteMemberByEmail && (
				<InviteMemberByEmail
					openModal={isOpenInviteMemberByEmail}
					handleCloseModalInviteClass={handleCloseModalInviteClass}
				/>
			)}
		</div>
	);
};
export default ClassroomMember;
