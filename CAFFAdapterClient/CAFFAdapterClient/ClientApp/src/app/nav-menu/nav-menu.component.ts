import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { SessionData } from '../services/sessionData';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  isUserLoggedIn: boolean = false;
  isAdminLoggedIn: boolean = false;

  constructor(private authService: AuthenticationService, private router: Router) {
    this.authService.adminLoggedIn.subscribe(res => this.changeToAdminNavBar(res));
    this.authService.userLoggedIn.subscribe(res => this.changeToUserNavBar(res));
  }

  ngOnInit(): void {
    this.loadSessionData();
  }

  loadSessionData() {
    this.isAdminLoggedIn = sessionStorage.getItem(SessionData.ADMIN_LOGGED_IN) != null ? true : false;
    this.isUserLoggedIn = sessionStorage.getItem(SessionData.USER_LOGGED_IN) != null ? true : false;
  }

  changeToAdminNavBar(res) {
    this.isAdminLoggedIn = res;
  }

  changeToUserNavBar(res) {
    this.isUserLoggedIn = res;
  }

  logout() {
    sessionStorage.clear();
    this.isUserLoggedIn = false;
    this.isAdminLoggedIn = false;
    this.router.navigate(['']);
  }
  
  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
