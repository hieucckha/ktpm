import axios from "../api/AxiosClient";
import {
	ApproveRequestDto,
	CreateCommentRequestDto,
	CreateRequestDto,
} from "../api/store/request/interface";

export class RequestService {
	public static async getRequestById(id: string) {
		const response = await axios.get(`/api/request/${id}`);
		return response.data;
	}

	public static async createRequest(request: CreateRequestDto) {
		const response = await axios.post(`/api/request`, request);
		return response.data;
	}

	public static async createCommentRequest({
		req_id,
		...rest
	}: CreateCommentRequestDto) {
		const response = await axios.post(`/api/request/${req_id}/comment`, {
			...rest,
		});
		return response.data;
	}

	public static async approveRequest({ req_id, ...rest }: ApproveRequestDto) {
		const response = await axios.post(`/api/request/${req_id}/approve`, {
			...rest,
		});
		return response.data;
	}

	public static async rejectRequest(req_id: number) {
		const response = await axios.post(`/api/request/${req_id}/reject`);
		return response.data;
	}
}
