import { useMutation, useQueryClient } from "@tanstack/react-query";
import { RequestService } from "../../../services/request.service";

export const useCreateGradeRequest = () => {
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: RequestService.createRequest,
	});
};

export const useAddRequestComment = (reqId?: string) => {
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: RequestService.createCommentRequest,
		onSuccess: () =>
			queryClient.invalidateQueries({
				queryKey: ["request", reqId],
				exact: true,
			}),
	});
};

export const useApproveRequest = (classId?: string) => {
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: RequestService.approveRequest,
		onSuccess: () =>
			queryClient.invalidateQueries({
				queryKey: ["class", classId],
			}),
	});
};

export const useRejectRequest = (classId?: string) => {
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: RequestService.rejectRequest,
		onSuccess: () =>
			queryClient.invalidateQueries({
				queryKey: ["class", classId],
			}),
	});
};
