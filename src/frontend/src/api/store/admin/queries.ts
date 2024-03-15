import classService from '../../../services/class.service';
import { UseQueryResult, useQuery } from "@tanstack/react-query";
import { ClassListQuery, userListQuery } from './interface';


export const  classQueryWithoutParams = (): UseQueryResult<ClassListQuery[]> => {
	const queryData = useQuery({
		queryKey: ["admin-classes"],
		queryFn: () => classService.getAllClassWithoutParams(),
	});
	return queryData;
};
export const userQueryResult =(): UseQueryResult<userListQuery[]> => {
	const queryData = useQuery({
		queryKey: ["admin-users"],
		queryFn: () => classService.userQueryResult(),
	});
	return queryData;
}
