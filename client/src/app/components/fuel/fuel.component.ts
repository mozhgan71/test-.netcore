import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { CalcCost } from 'src/app/models/calccost.model';
import { Car } from 'src/app/models/car.model';
import { RateFuel } from 'src/app/models/rate-fuel.model';

@Component({
  selector: 'app-fuel',
  templateUrl: './fuel.component.html',
  styleUrls: ['./fuel.component.scss']
})
export class FuelComponent {
  userRes: CalcCost | undefined;
  showError: CalcCost | undefined;

  results: CalcCost[] | undefined;
  reports: CalcCost[] | undefined;
  formResults: Car[] | undefined;
  res: Car | undefined;
  nameCar: string | undefined;
  payable: number | null | undefined;
  rateFuel: RateFuel | undefined;
  rateResults: RateFuel[] | undefined;
  dateToCheck: string | undefined;
  nowDate = Date();

  constructor(private fb: FormBuilder, private http: HttpClient) {
    // debugger;
    this.showAll();
    this.createDate();
    this.showAllInForm();
    // this.checkDate();
    console.log(this.nowDate);

  }
  createDate(): void {
    let dataObj = new Date();
    let month = dataObj.getUTCMonth() + 1;
    let day = dataObj.getUTCDate();
    let year = dataObj.getFullYear();

    this.nowDate = year + "/" + month + "/" + day;
  }

  //#region Create Form Group/controler (AbstractControl)
  userFg = this.fb.group({ // formGroup
    driverNameCtrl: ['', [Validators.required]],
    dateCtrl: ['',],
    amountCtrl: ['', [Validators.required]],
    rateCtrl: ['', [Validators.required]],
    typeCarCtrl: ['', [Validators.required]],
    payableCtrl: ['', [Validators.required]],
  });
  //#endregion

  //#region Forms Properties
  get DriverNameCtrl(): FormControl {
    return this.userFg.get('driverNameCtrl') as FormControl;
  }
  get DateCtrl(): FormControl {
    return this.userFg.get('dateCtrl') as FormControl;
  }
  get AmountCtrl(): FormControl {
    return this.userFg.get('amountCtrl') as FormControl;
  }
  get RateCtrl(): FormControl {
    return this.userFg.get('rateCtrl') as FormControl;
  }
  get TypeCarCtrl(): FormControl {
    return this.userFg.get('typeCarCtrl') as FormControl;
  }
  get PayableCtrl(): FormControl {
    return this.userFg.get('payableCtrl') as FormControl;
  }

  //#endregion

  //#region Methods
  calcPay(): void {
    this.payable = this.AmountCtrl.value * this.RateCtrl.value
    this.PayableCtrl.setValue(this.payable);
    console.log(this.payable);
  }
  findNameCar(id: string): void {
    // debugger;
    this.http.get<Car>('http://localhost:5000/api/car/get-by-id/' + id).subscribe(
      {
        next: response => {
          this.res = response
          console.log(this.res);
          this.nameCar = this.res.typeCar;
          this.TypeCarCtrl.setValue(this.nameCar);
        }
      }
    );
  }

  submitUser(): void {
    console.log(this.userFg.value);
    var date = Date.now;
    this.DateCtrl.setValue(date.toString());

    let item: CalcCost = {
      driverName: this.DriverNameCtrl.value,
      dateRefueling: (Date.now).toString(),
      amount: this.AmountCtrl.value,
      rate: this.RateCtrl.value,
      typeCar: this.TypeCarCtrl.value,
      payable: this.PayableCtrl.value
    }

    this.http.post<CalcCost>('http://localhost:5000/api/calccost/add-result', item).subscribe(
      {
        next: res => {
          this.userRes = res;
          console.log(res);
        },
        error: err => {
          this.showError = err.error;
          alert(this.showError);
        }
      }
    );
    this.userFg.reset();
  }
  //#endregion 

  showAll(): void {
    this.http.get<CalcCost[]>('http://localhost:5000/api/calccost/').subscribe(
      {
        next: response => {
          this.results = response
          console.log(this.results);

        }
      }
    );
  }

  showReport(id: string): void {
    sessionStorage.setItem('id-result', id);

    this.http.get<CalcCost[]>('http://localhost:5000/api/calccost/get-by-id/' + id).subscribe(
      {
        next: response => {
          this.reports = response
          console.log(this.reports);
        }
      }
    );
  }

  showAllInForm(): void {
    this.http.get<Car[]>('http://localhost:5000/api/car/').subscribe(
      {
        next: response => {
          this.formResults = response
          console.log(this.formResults);
        }
      }
    );
  }

  // checkDate() {
  //   this.http.get<RateFuel[]>('http://localhost:5000/api/fuelrate/').subscribe(
  //     {
  //       next: response => {
  //         this.rateResults = response
  //         console.log(this.rateResults);
  //         this.rateResults.forEach(item => {
  //           let startDate = item.fromDate;
  //           let endDate = item.untilDate;
  //           let dateToCheck = Date().toString();
  //           console.log(startDate);
  //           console.log(endDate);
  //           console.log(dateToCheck);
  //         });
  //       }
  //     }
  //   );
  // }
}
