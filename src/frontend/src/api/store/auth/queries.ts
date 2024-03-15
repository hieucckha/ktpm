import { useQuery } from "@tanstack/react-query";

import UserService from "../../../services/user.service";

import { UserProfileDto } from "./interface";

const useAuthQuery = <TData = UserProfileDto>(
	select?: (data: UserProfileDto) => TData
) => {
	const queryData = useQuery({
		queryKey: ["my-profile"],
		queryFn: UserService.getProfile,
		select: select,
	});

	return queryData;
};

export default useAuthQuery;
