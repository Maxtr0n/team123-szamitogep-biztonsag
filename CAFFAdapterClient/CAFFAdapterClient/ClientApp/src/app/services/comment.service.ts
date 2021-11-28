import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GetCommentContainerResponse } from '../entities/comment/GetCommentContainerResponse';
import { EndPoint } from './endpoints';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  url = EndPoint.COMMENTS_URL;

  constructor(private http: HttpClient) { }

  getCommentsForGif(gifId: string) {
    return this.http.get(this.url + '?gifId=' + gifId).toPromise();
  }

  addCommentForGif(gifId: number, comment: string) {
    var requestBody = {
      message: comment
    };
    return this.http.post(EndPoint.BASE_URL_2 + '/CaffFiles/' + gifId + '/comments', requestBody).toPromise();
  }

  getCommentsByGifId(gifId: number) {
    return this.http.get<GetCommentContainerResponse>(EndPoint.BASE_URL_2 + '/CaffFiles/getCommentsByGifId/' + gifId).toPromise();
  }

  deleteComment(commentId: number) {
    return this.http.delete(EndPoint.BASE_URL_2 + '/user' + '/deleteComment/' + commentId).toPromise();
  }
}
