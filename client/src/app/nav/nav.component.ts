import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model:any = {}
  //currentUser$:Observable<User>
 // loggedIn:boolean
  constructor(public account:AccountService) { }

  ngOnInit(): void {
    //this.getCurrentUser()
    //this.currentUser$ = this.account.currentUser$
  }
  login(){

    this.account.login(this.model).subscribe(response=>{
      console.log(response)
    //  this.loggedIn = true
    },
    err=>{
      console.error(err)
    }

    )
  }
  logout(){
    this.account.logout()
   // this.loggedIn = false

  }
  // getCurrentUser(){
  //   this.account.currentUser$.subscribe(user=>{
  //     this.loggedIn = !!user
  //   },
  //     error=>{console.log(console.error())}

  //   )
  // }
}
