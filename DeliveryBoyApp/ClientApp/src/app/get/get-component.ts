import { Component } from '@angular/core';
import { DataService, TipDto } from '../DataService';

@Component({
  selector: 'get-component',
  templateUrl: './get-component.html',
})
export class GetComponent {
  public _DataService: DataService;

  public streetNumber: number;
  public streetName: string;
  public zipcode: number;

  public firstRun: boolean = true;
  public gettingTipData: boolean = false;

  public tipData: TipDto;

  constructor(dataService: DataService) {

    this._DataService = dataService;

    this.tipData = new TipDto();
  }

  ngOnInit() {
    this._DataService.TipDataReceived.subscribe(x => this.GotTipData(x));
  }

  public getTips() {
    if (this.streetNumber != null && this.streetName != null && this.zipcode != null) {
      this.gettingTipData = true;
      this._DataService.getTipData(this.streetNumber, this.streetName, this.zipcode);
    }
    
  }

  private GotTipData(data: TipDto) {
    if (data != null) {
      this.tipData = data;
      this.gettingTipData = false;
      this.firstRun = false;
    }
  }
}
