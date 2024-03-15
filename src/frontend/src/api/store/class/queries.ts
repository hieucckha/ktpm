import classService from "../../../services/class.service";
import type { ClassDetail, ClassQuery, gradeAll } from "./interface";
import { UseQueryResult, useQuery } from "@tanstack/react-query";

export const useGetAllClassesQuery = (
	user_id?: number
): UseQueryResult<ClassQuery[]> => {
	const queryData = useQuery({
		queryKey: ["classes"],
		queryFn: () => classService.getAllClass(user_id),
		enabled: !!user_id,
	});
	return queryData;
};
export const classDetailQuery = (id: string): UseQueryResult<ClassDetail> => {
	const queryData = useQuery({
		queryKey: ["class", id],
		queryFn: () => classService.getClassDetail(id),
		enabled: !!id,
	});
	return queryData;
};
export const listGradeAllClassQuery = (
	id: string
): UseQueryResult<gradeAll> => {
	const queryData = useQuery({
		queryKey: ["class-grade", id],
		queryFn: () => classService.getAllGrade(id),
		enabled: !!id,
	});
	return queryData;
};
export const listGradeOneStudentQuery = (
	id: string,
	studentId: string
): UseQueryResult<gradeAll> => {
	const queryData = useQuery({
		queryKey: ["class-grade", { id, studentId }],
		queryFn: () => classService.getOneGradeStudent(id, studentId),
		enabled: !!id && !!studentId && studentId !== "",
	});
	return queryData;
};
