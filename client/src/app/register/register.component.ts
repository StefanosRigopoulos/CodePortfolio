import { Component, OnInit, } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Lists } from '../_lists/lists';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {
  registerForm: FormGroup = new FormGroup({});
  step = 1;
  maxDate: Date = new Date();
  validationErrors: string[];

  constructor(private accountService: AccountService, private router: Router, private toastr: ToastrService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 13);
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(12)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(12), this.matchValuesValidator('password')]],
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      dateofbirth: ['', Validators.required],
      gender: ['', Validators.required],
      country: ['', Validators.required],
      codelanguage: ['', Validators.required],
      facebook: [''],
      twitter: [''],
      github: [''],
      linkedin: [''],
      photourl: ['https://images.unsplash.com/photo-1618614944895-fc409a83ad80?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=880&q=80']
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    });
  }

  matchValuesValidator(matchTo: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const password = control.parent?.get(matchTo)?.value;
      return password !== control.value ? { notMatching: true } : null;
    };
  }

  get control() { return this.registerForm.controls }
  get codeList() { return Lists.CodeLanguageList }
  get countryList() { return Lists.CountryList }

  next() {
    this.step++;
  }
  previous(){
    this.step--;
  }

  submit() {
    const dob = this.getDateOnly(this.registerForm.controls['dateofbirth'].value);
    const values = {...this.registerForm.value, dateofbirth: dob};
    var checkbox = <HTMLInputElement> document.getElementById("checkForRegister");
    if (checkbox.checked == true) {
      this.accountService.register(values).subscribe({
        next: () => {
          this.router.navigateByUrl('/');
        },
        error: error => {
          this.validationErrors = error;
        }
      });
    } else {
      this.toastr.error("You have to accept the Terms of Service!");
    }
  }

  private getDateOnly(dob: string | undefined) {
    if (!dob) return;
    let theDob = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())).toISOString().slice(0,10);
  }
}
