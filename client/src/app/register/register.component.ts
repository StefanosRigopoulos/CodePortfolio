import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model:any = {};

  constructor(private accountService: AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register() {
    var checkbox = <HTMLInputElement> document.getElementById("checkForRegister");
    if (checkbox.checked == true) {
      this.accountService.register(this.model).subscribe({
        next: () => {
          this.router.navigateByUrl('/');
          this.cancel();
        },
        error: error => {
          this.toastr.error(error.error);
        }
      });
    } else {
      this.toastr.error("You have to accept the Terms Of Service!");
    }
  }

  cancel(){
    this.cancelRegister.emit(false);
  }
}
