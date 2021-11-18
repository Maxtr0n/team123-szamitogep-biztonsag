export class EndPoint {
    public static BASE_URL: string = 'http://localhost:3000';
    public static BASE_URL_2: string = 'https://localhost:44343';
    public static LOGIN_URL: string = EndPoint.BASE_URL_2 + '/account/login';
    public static COMMENTS_URL: string = EndPoint.BASE_URL + '/comments';
    public static REGISTER_URL: string = EndPoint.BASE_URL_2 +  '/account/register';
}