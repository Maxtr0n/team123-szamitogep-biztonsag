import { GetCommentResponse } from "./comment/GetCommentResponse";

export class CommentsDialogData {
    comments: GetCommentResponse[];
    newComment: string;
}