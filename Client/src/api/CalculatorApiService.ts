import { BaseApiService } from './BaseApiService';

type ApiResult<T> = {
  isSuccess: boolean;
  value: T;
  errorMessage?: string;
};

export enum Gender {
  Male = 1,
  Female = 2,
}

export enum BmiCategory {
  Underweight = 1,
  Normal = 2,
  Overweight = 3,
  Obesity = 4,
}

export enum BodyFatCategory {
  VeryLow = 1,
  Athletic = 2,
  Fitness = 3,
  Average = 4,
  High = 5,
}

export enum CalculatorFormula {
  Epley = 1,
  MifflinStJeor = 2,
  Ymca = 3,
  Devine = 4,
}

export interface BmiRequestDto {
  weightKg: number;
  heightCm: number;
}

export interface BmiResultDto {
  bmi: number;
  category: BmiCategory;
  healthyWeightMinKg: number;
  healthyWeightMaxKg: number;
}

export interface OneRepMaxRequestDto {
  weightKg: number;
  reps: number;
}

export interface OneRepMaxResultDto {
  oneRepMaxKg: number;
  formula: CalculatorFormula;
}

export interface BmrRequestDto {
  weightKg: number;
  heightCm: number;
  age: number;
  gender: Gender;
  activityFactor?: number;
}

export interface BmrResultDto {
  basalMetabolicRate: number;
  maintenanceCalories: number;
  formula: CalculatorFormula;
}

export interface YmcaBodyFatRequestDto {
  weightKg: number;
  waistCm: number;
  gender: Gender;
}

export interface YmcaBodyFatResultDto {
  bodyFatPercentage: number;
  category: BodyFatCategory;
  formula: CalculatorFormula;
}

export interface IdealWeightRequestDto {
  heightCm: number;
  gender: Gender;
}

export interface IdealWeightResultDto {
  idealWeightKg: number;
  rangeMinKg: number;
  rangeMaxKg: number;
  formula: CalculatorFormula;
}

export class CalculatorApiService extends BaseApiService {
  static async calculateBmi(dto: BmiRequestDto): Promise<BmiResultDto> {
    const result = await this.postAsync<ApiResult<BmiResultDto>, BmiRequestDto>('/calculators/bmi', dto);
    return result.value;
  }

  static async calculateOneRepMax(dto: OneRepMaxRequestDto): Promise<OneRepMaxResultDto> {
    const result = await this.postAsync<ApiResult<OneRepMaxResultDto>, OneRepMaxRequestDto>('/calculators/one-rep-max', dto);
    return result.value;
  }

  static async calculateBmr(dto: BmrRequestDto): Promise<BmrResultDto> {
    const result = await this.postAsync<ApiResult<BmrResultDto>, BmrRequestDto>('/calculators/bmr', dto);
    return result.value;
  }

  static async calculateYmcaBodyFat(dto: YmcaBodyFatRequestDto): Promise<YmcaBodyFatResultDto> {
    const result = await this.postAsync<ApiResult<YmcaBodyFatResultDto>, YmcaBodyFatRequestDto>('/calculators/body-fat/ymca', dto);
    return result.value;
  }

  static async calculateIdealWeight(dto: IdealWeightRequestDto): Promise<IdealWeightResultDto> {
    const result = await this.postAsync<ApiResult<IdealWeightResultDto>, IdealWeightRequestDto>('/calculators/ideal-weight', dto);
    return result.value;
  }
}
