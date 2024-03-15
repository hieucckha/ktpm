import { useMutation, useQueryClient } from "@tanstack/react-query";

import classService from "../../../services/class.service";
import type { ClassDto, ClassQuery, EditUserDto } from "./interface";
import { useParams } from "react-router-dom";

export const useCreateClassMutation = () => {
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: (classData: ClassDto) => classService.createClass(classData),
		retry: false,
		onSuccess() {
			queryClient.invalidateQueries({ queryKey: ["classes"] });
		},
	});
};

export const useJoinClassMutation = () => {
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: (classData: { code: string }) =>
			classService.joinClassByCode(classData),
		retry: false,
		onSuccess() {
			queryClient.invalidateQueries({ queryKey: ["classes"] });
		},
	});
};
export const useInviteClassMutationByEmail = () => {
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: (classData: { email: string; courseId: string }) =>
			classService.inviteClassByEmail(classData),
		retry: false,
		onSuccess() {
			queryClient.invalidateQueries({ queryKey: ["classes"] });
		},
	});
};

export const useEditGradeMutation = () => {
	const queryClient = useQueryClient();
	const { id: classId } = useParams();
	if (!classId) throw new Error("No class id");

	return useMutation({
		mutationKey: ["edit-grade-student"],
		mutationFn: classService.editGradeStudent,
		onSuccess() {
			return queryClient.refetchQueries({
				queryKey: ["class-grade", classId],
				exact: true,
			});
		},
	});
};

export const useEditClassMutation = (classId?: string) => {
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: (classQuery: ClassQuery) => classService.editClass(classQuery),
		retry: false,
		onSuccess() {
			queryClient.invalidateQueries({ queryKey: ["class", classId] });
		},
	});
};
export const lockUserMutation = () => {
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: (id: number) => classService.lockUser(id),
		retry: false,
		onSuccess() {
			queryClient.invalidateQueries({ queryKey: ["admin-users"] });
		},
	});
}
export const unlockUserMutation = () => {
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: (id: number) => classService.unlockUser(id),
		retry: false,
		onSuccess() {
			queryClient.invalidateQueries({ queryKey: ["admin-users"] });
		},
	});
}



export const EditUserMutation = () => {
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: ({id , ...rest}: EditUserDto) => classService.editUser(id, rest),
		retry: false,
		onSuccess() {
			queryClient.invalidateQueries({ queryKey: ["admin-users"] });
		},
	});
}
