import { useMutation, useQueryClient } from "@tanstack/react-query";

import classService from '../../../services/class.service';
import { gradeCompositions } from "./interface";
import { newGradeCompositions } from "../class/interface";

export const useUpdateOrderGradeComposit = () => {
    return useMutation({
        mutationFn: (gradeCompositions:gradeCompositions[]) => classService.updateOrderGradeComposit(gradeCompositions),
        retry: false,
        onSuccess() {
            // queryClient.invalidateQueries({ queryKey: ['class'] });
        },
    });
}

export const useUpdateGradeColumn = () => {
    return useMutation({
        mutationFn: (gradeComposition:gradeCompositions) => classService.updateGradeColumn(gradeComposition),
        retry: false,
        onSuccess() {
            // queryClient.invalidateQueries({ queryKey: ['class'] });
        },
    });
}
export const useAddNewGradeComposit = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (composition:newGradeCompositions) => classService.addNewGradeComposit(composition),
        retry: false,
        onSuccess() {
            queryClient.invalidateQueries({ queryKey: ['class'] });
        },
    });
}
export const useDeleteNewGradeComposit = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (compositionId:number) => classService.deleteGradeComposit(compositionId),
        retry: false,
        onSuccess() {
            queryClient.invalidateQueries({ queryKey: ['class'] });
        },
    });
}
export const useApproveGradeComposit = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (compositionId:number) => classService.approveGradeComposition(compositionId),
        retry: false,
        onSuccess() {
            queryClient.invalidateQueries({ queryKey: ['class'] });
        },
    });
}