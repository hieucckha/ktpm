export interface CreateRequestDto {
	reason: string;
	exceptedGrade: number;
	gradeCompositionId: number;
}

export interface CreateCommentRequestDto {
	req_id: string;
	comment: string;
}

export interface ApproveRequestDto {
	req_id: string;
	gradeValue: string;
}
