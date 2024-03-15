import { useQueries, useQuery } from "@tanstack/react-query";
import { RequestService } from "../../../services/request.service";
import { RequestDto } from "../class/interface";

export const useGetRequest = <TData = RequestDto>(
	id: string,
	select?: (data: RequestDto) => TData
) => {
	return useQuery({
		queryKey: ["request", id],
		queryFn: () => RequestService.getRequestById(id),
		select,
	});
};

export const useGetRequests = (requestIds?: string[]) => {
	return useQueries({
		queries: requestIds.map((id) => ({
			queryKey: ["request", id],
			queryFn: () => RequestService.getRequestById(id),
			enabled: requestIds && !!id,
		})),
	});
};
