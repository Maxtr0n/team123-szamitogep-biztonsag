import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
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

  addCommentForGif(difId: string, comment: string) {
    var userId = '';
  }
}
