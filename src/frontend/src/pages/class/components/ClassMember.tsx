import {  Flex, Space, Avatar } from "antd";
import { UserProfileDto } from "../../../api/store/auth/interface";
import { FC } from "react";

interface MemberCardProps extends Partial<UserProfileDto> {
  isOwner?: boolean;
  isStudent?: boolean;
  isTeacher?: boolean;
  className?: string;
}
const ClassMember: FC<MemberCardProps> = ({
  fullName,
  className = 'text-base px-4 h-[52px] md:h-[64px]',
}) => {

  return (
    <Flex className={className} align="center" justify="start" gap={'middle'}>
      <Flex flex={1} justify="start">
        <Space align="center" size="middle">
          <Avatar className="hidden sm:block object-cover scale-150" shape="circle" src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ39lLZZSixexryn9Y9tVucQY52-nOlNJfqNperhD4pZUZGAGLnBi5rhWiHFXJCUgfeKnQ&usqp=CAU"/> 
          <span className="text-black text-xl !font-bold leading-5">{fullName} </span>
        </Space>
      </Flex>
      {/* {dropDownRender()} */}
    </Flex>
  );
}

export default ClassMember;