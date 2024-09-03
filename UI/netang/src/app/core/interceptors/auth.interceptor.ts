import { HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const cookieService = inject(CookieService); // Initialize the CookieService

  const authToken = cookieService.get('Authorization');

  const shouldAddAuth = (url: string): boolean => {
    return url.indexOf('addAuth=true', 0) > -1;
  };
  
  if (shouldAddAuth(req.urlWithParams)) {
    const authRequest = req.clone({
      setHeaders: {
        Authorization: authToken, // Retrieve the 'Authorization' data from the cookie
      },
    });

    return next(authRequest);
  }

  return next(req);
};
