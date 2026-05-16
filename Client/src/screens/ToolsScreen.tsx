// src/screens/ToolsScreen.tsx
import React, { useMemo, useState } from 'react';
import {
  Alert,
  Image,
  Pressable,
  ScrollView,
  Text,
  View,
} from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import BackButton, { backButtonSpacing } from '../components/BackButton';
import {
  BmiCategory,
  BodyFatCategory,
  CalculatorApiService,
  CalculatorFormula,
  Gender,
} from '../api/CalculatorApiService';
import { Spacing } from '../theme/theme';
import { useTheme } from '../context/ThemeContext';
import FormTextInput from '../components/FormTextInput';
import PrimaryButton from '../components/PrimaryButton';
import { styles } from './ToolsScreen.styles';

type CalculatorId = 'bmi' | 'bmr' | 'one-rm' | 'body-fat' | 'ideal-weight';

type ResultRow = {
  label: string;
  value: string;
};

type CalculatorConfig = {
  id: CalculatorId;
  name: string;
  description: string;
  image: string;
  icon: string;
  accent: string;
};

const calculators: CalculatorConfig[] = [
  {
    id: 'bmi',
    name: 'Kalkulator BMI',
    description: 'Oblicz wskaźnik masy ciała i zdrowy zakres wagi.',
    image: 'https://images.unsplash.com/photo-1750521279808-f66baaed923d?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=600',
    icon: 'activity',
    accent: '#2563eb',
  },
  {
    id: 'bmr',
    name: 'Kalkulator kalorii',
    description: 'Sprawdź BMR oraz dzienne kalorie utrzymania.',
    image: 'https://images.unsplash.com/photo-1670164747721-d3500ef757a6?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=600',
    icon: 'zap',
    accent: '#f59e0b',
  },
  {
    id: 'one-rm',
    name: 'Kalkulator 1RM',
    description: 'Oblicz szacowane maksymalne obciążenie na jedno powtórzenie.',
    image: 'https://images.unsplash.com/photo-1750521280541-bbf9d813a890?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=600',
    icon: 'trending-up',
    accent: '#ef4444',
  },
  {
    id: 'body-fat',
    name: 'Poziom body fat',
    description: 'Oszacuj procent tkanki tłuszczowej metodą YMCA.',
    image: 'https://images.unsplash.com/photo-1571019614242-c5c5dee9f50b?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=600',
    icon: 'pie-chart',
    accent: '#8b5cf6',
  },
  {
    id: 'ideal-weight',
    name: 'Idealna waga',
    description: 'Wylicz wagę docelową i praktyczny zakres według wzrostu.',
    image: 'https://images.unsplash.com/photo-1523362628745-0c100150b504?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=600',
    icon: 'target',
    accent: '#16a34a',
  },
];

const activityOptions = [
  { label: 'Niska', value: 1.2 },
  { label: 'Lekka', value: 1.375 },
  { label: 'Średnia', value: 1.55 },
  { label: 'Wysoka', value: 1.725 },
  { label: 'Bardzo wysoka', value: 1.9 },
];

const bmiCategoryLabels: Record<BmiCategory, string> = {
  [BmiCategory.Underweight]: 'Niedowaga',
  [BmiCategory.Normal]: 'Waga prawidłowa',
  [BmiCategory.Overweight]: 'Nadwaga',
  [BmiCategory.Obesity]: 'Otyłość',
};

const bodyFatCategoryLabels: Record<BodyFatCategory, string> = {
  [BodyFatCategory.VeryLow]: 'Bardzo niski',
  [BodyFatCategory.Athletic]: 'Sportowy',
  [BodyFatCategory.Fitness]: 'Fitness',
  [BodyFatCategory.Average]: 'Przeciętny',
  [BodyFatCategory.High]: 'Podwyższony',
};

const formulaLabels: Record<CalculatorFormula, string> = {
  [CalculatorFormula.Epley]: 'Epley',
  [CalculatorFormula.MifflinStJeor]: 'Mifflin-St Jeor',
  [CalculatorFormula.Ymca]: 'YMCA',
  [CalculatorFormula.Devine]: 'Devine',
};

const parseNumber = (value: string) => {
  const parsed = Number(value.replace(',', '.'));
  return Number.isFinite(parsed) ? parsed : undefined;
};

export default function ToolsScreen({ navigation }: any) {
  const insets = useSafeAreaInsets();
  const { colors, isDark } = useTheme();

  const [activeCalculator, setActiveCalculator] = useState<CalculatorId | null>(null);
  const [weight, setWeight] = useState('');
  const [height, setHeight] = useState('');
  const [age, setAge] = useState('');
  const [waist, setWaist] = useState('');
  const [reps, setReps] = useState('');
  const [gender, setGender] = useState<Gender>(Gender.Male);
  const [activityFactor, setActivityFactor] = useState(1.55);
  const [isLoading, setIsLoading] = useState(false);
  const [resultRows, setResultRows] = useState<ResultRow[]>([]);
  const [resultHeadline, setResultHeadline] = useState<string | null>(null);

  const selectedCalculator = useMemo(
    () => calculators.find(calculator => calculator.id === activeCalculator) ?? null,
    [activeCalculator],
  );

  const resetResult = () => {
    setResultRows([]);
    setResultHeadline(null);
  };

  const openCalculator = (id: CalculatorId) => {
    setActiveCalculator(id);
    resetResult();
  };

  const goBackFromCalculator = () => {
    setActiveCalculator(null);
    resetResult();
  };

  const requireNumber = (value: string, label: string) => {
    const parsed = parseNumber(value);
    if (parsed === undefined) {
      throw new Error(`Podaj prawidłową wartość: ${label}.`);
    }
    return parsed;
  };

  const handleCalculate = async () => {
    if (!activeCalculator) return;

    setIsLoading(true);
    resetResult();

    try {
      if (activeCalculator === 'bmi') {
        const response = await CalculatorApiService.calculateBmi({
          weightKg: requireNumber(weight, 'waga'),
          heightCm: requireNumber(height, 'wzrost'),
        });

        setResultHeadline(`${response.bmi.toFixed(1)} BMI`);
        setResultRows([
          { label: 'Kategoria', value: bmiCategoryLabels[response.category] },
          { label: 'Zdrowy zakres', value: `${response.healthyWeightMinKg.toFixed(1)} - ${response.healthyWeightMaxKg.toFixed(1)} kg` },
        ]);
      }

      if (activeCalculator === 'bmr') {
        const response = await CalculatorApiService.calculateBmr({
          weightKg: requireNumber(weight, 'waga'),
          heightCm: requireNumber(height, 'wzrost'),
          age: Math.round(requireNumber(age, 'wiek')),
          gender,
          activityFactor,
        });

        setResultHeadline(`${Math.round(response.maintenanceCalories)} kcal`);
        setResultRows([
          { label: 'BMR', value: `${Math.round(response.basalMetabolicRate)} kcal` },
          { label: 'Formuła', value: formulaLabels[response.formula] },
        ]);
      }

      if (activeCalculator === 'one-rm') {
        const response = await CalculatorApiService.calculateOneRepMax({
          weightKg: requireNumber(weight, 'ciężar'),
          reps: Math.round(requireNumber(reps, 'powtórzenia')),
        });

        setResultHeadline(`${response.oneRepMaxKg.toFixed(1)} kg`);
        setResultRows([{ label: 'Formuła', value: formulaLabels[response.formula] }]);
      }

      if (activeCalculator === 'body-fat') {
        const response = await CalculatorApiService.calculateYmcaBodyFat({
          weightKg: requireNumber(weight, 'waga'),
          waistCm: requireNumber(waist, 'obwód pasa'),
          gender,
        });

        setResultHeadline(`${response.bodyFatPercentage.toFixed(1)}%`);
        setResultRows([
          { label: 'Kategoria', value: bodyFatCategoryLabels[response.category] },
          { label: 'Formuła', value: formulaLabels[response.formula] },
        ]);
      }

      if (activeCalculator === 'ideal-weight') {
        const response = await CalculatorApiService.calculateIdealWeight({
          heightCm: requireNumber(height, 'wzrost'),
          gender,
        });

        setResultHeadline(`${response.idealWeightKg.toFixed(1)} kg`);
        setResultRows([
          { label: 'Zakres', value: `${response.rangeMinKg.toFixed(1)} - ${response.rangeMaxKg.toFixed(1)} kg` },
          { label: 'Formuła', value: formulaLabels[response.formula] },
        ]);
      }
    } catch (error: any) {
      Alert.alert('Błąd', error.message ?? 'Nie udało się wykonać obliczeń.');
    } finally {
      setIsLoading(false);
    }
  };

  const renderNumberInput = (
    label: string,
    value: string,
    onChangeText: (value: string) => void,
    placeholder: string,
    suffix: string,
  ) => (
    <FormTextInput
      label={label}
      value={value}
      onChangeText={onChangeText}
      keyboardType="numeric"
      placeholder={placeholder}
      suffix={suffix}
    />
  );

  const renderGenderPicker = () => (
    <View style={styles.inputGroup}>
      <Text style={[styles.inputLabel, { color: colors.foreground }]}>Płeć</Text>
      <View style={[styles.segmentedControl, { backgroundColor: colors.card, borderColor: colors.border }]}>
        {[
          { label: 'Mężczyzna', value: Gender.Male },
          { label: 'Kobieta', value: Gender.Female },
        ].map(option => {
          const isSelected = gender === option.value;
          return (
            <Pressable
              key={option.value}
              style={[styles.segmentButton, isSelected && { backgroundColor: selectedCalculator?.accent ?? '#2563eb' }]}
              onPress={() => setGender(option.value)}
            >
              <Text style={[styles.segmentText, { color: isSelected ? '#ffffff' : colors.foreground }]}>
                {option.label}
              </Text>
            </Pressable>
          );
        })}
      </View>
    </View>
  );

  const renderActivityPicker = () => (
    <View style={styles.inputGroup}>
      <Text style={[styles.inputLabel, { color: colors.foreground }]}>Aktywność</Text>
      <View style={styles.activityGrid}>
        {activityOptions.map(option => {
          const isSelected = activityFactor === option.value;
          return (
            <Pressable
              key={option.value}
              style={[
                styles.activityChip,
                { backgroundColor: colors.card, borderColor: colors.border },
                isSelected && { backgroundColor: '#f59e0b', borderColor: '#f59e0b' },
              ]}
              onPress={() => setActivityFactor(option.value)}
            >
              <Text style={[styles.activityText, { color: isSelected ? '#ffffff' : colors.foreground }]}>
                {option.label}
              </Text>
              <Text style={[styles.activityValue, { color: isSelected ? 'rgba(255,255,255,0.85)' : colors.mutedForeground }]}>
                x{option.value}
              </Text>
            </Pressable>
          );
        })}
      </View>
    </View>
  );

  const renderCalculatorFields = () => {
    switch (activeCalculator) {
      case 'bmi':
        return (
          <>
            {renderNumberInput('Waga', weight, setWeight, '75', 'kg')}
            {renderNumberInput('Wzrost', height, setHeight, '180', 'cm')}
          </>
        );
      case 'bmr':
        return (
          <>
            {renderNumberInput('Waga', weight, setWeight, '75', 'kg')}
            {renderNumberInput('Wzrost', height, setHeight, '180', 'cm')}
            {renderNumberInput('Wiek', age, setAge, '30', 'lat')}
            {renderGenderPicker()}
            {renderActivityPicker()}
          </>
        );
      case 'one-rm':
        return (
          <>
            {renderNumberInput('Ciężar', weight, setWeight, '100', 'kg')}
            {renderNumberInput('Powtórzenia', reps, setReps, '5', 'rep')}
          </>
        );
      case 'body-fat':
        return (
          <>
            {renderNumberInput('Waga', weight, setWeight, '75', 'kg')}
            {renderNumberInput('Obwód pasa', waist, setWaist, '84', 'cm')}
            {renderGenderPicker()}
          </>
        );
      case 'ideal-weight':
        return (
          <>
            {renderNumberInput('Wzrost', height, setHeight, '180', 'cm')}
            {renderGenderPicker()}
          </>
        );
      default:
        return null;
    }
  };

  if (selectedCalculator) {
    return (
      <View style={[styles.container, { backgroundColor: colors.background }]}>
        <View style={[styles.header, { paddingTop: insets.top + Spacing.sm }]}>
          <BackButton onPress={goBackFromCalculator} style={backButtonSpacing} />
          <View style={styles.headerTitleRow}>
            <View style={[styles.headerIcon, { backgroundColor: `${selectedCalculator.accent}20` }]}>
              <Icon name={selectedCalculator.icon as any} size={18} color={selectedCalculator.accent} />
            </View>
            <View style={styles.headerText}>
              <Text style={[styles.title, { color: colors.foreground }]}>{selectedCalculator.name}</Text>
              <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>{selectedCalculator.description}</Text>
            </View>
          </View>
        </View>

        <ScrollView
          style={styles.content}
          contentContainerStyle={[styles.scrollContent, { paddingBottom: insets.bottom + Spacing.xl }]}
          keyboardShouldPersistTaps="handled"
          showsVerticalScrollIndicator={false}
        >
          <View style={[styles.formCard, { backgroundColor: colors.card, borderColor: colors.border }]}>
            {renderCalculatorFields()}

            <PrimaryButton
              title="Oblicz"
              color={selectedCalculator.accent}
              onPress={handleCalculate}
              loading={isLoading}
              style={styles.primaryButton}
            />
          </View>

          {resultHeadline && (
            <View style={[styles.resultBox, { backgroundColor: selectedCalculator.accent }]}>
              <Text style={styles.resultLabel}>Wynik</Text>
              <Text style={styles.resultValue}>{resultHeadline}</Text>
              <View style={styles.resultRows}>
                {resultRows.map(row => (
                  <View key={row.label} style={styles.resultRow}>
                    <Text style={styles.resultRowLabel}>{row.label}</Text>
                    <Text style={styles.resultRowValue}>{row.value}</Text>
                  </View>
                ))}
              </View>
            </View>
          )}

          <View style={[styles.infoBox, { backgroundColor: isDark ? colors.card : '#eff6ff', borderColor: colors.border }]}>
            <Icon name="info" size={16} color={selectedCalculator.accent} />
            <Text style={[styles.infoText, { color: colors.mutedForeground }]}>
              Wyniki mają charakter orientacyjny i nie zastępują konsultacji z lekarzem, dietetykiem ani trenerem.
            </Text>
          </View>
        </ScrollView>
      </View>
    );
  }

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <View style={[styles.header, { paddingTop: insets.top + Spacing.sm }]}>
        <BackButton onPress={() => navigation.navigate('TrainingHub')} style={backButtonSpacing} />
        <Text style={[styles.title, { color: colors.foreground }]}>Kalkulatory</Text>
        <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>
          Wybierz narzędzie, aby policzyć parametry treningowe i dietetyczne z API.
        </Text>
      </View>

      <ScrollView
        style={styles.content}
        contentContainerStyle={[styles.scrollContent, { paddingBottom: insets.bottom + Spacing.xl }]}
        showsVerticalScrollIndicator={false}
      >
        {calculators.map(calculator => (
          <Pressable
            key={calculator.id}
            style={[styles.calcCard, { backgroundColor: colors.card, borderColor: colors.border }]}
            onPress={() => openCalculator(calculator.id)}
          >
            <Image source={{ uri: calculator.image }} style={styles.calcImage} />
            <View style={styles.calcInfo}>
              <View style={styles.cardTitleRow}>
                <View style={[styles.cardIcon, { backgroundColor: `${calculator.accent}20` }]}>
                  <Icon name={calculator.icon as any} size={16} color={calculator.accent} />
                </View>
                <Text style={[styles.calcName, { color: colors.foreground }]}>{calculator.name}</Text>
              </View>
              <Text style={[styles.calcDesc, { color: colors.mutedForeground }]} numberOfLines={2}>
                {calculator.description}
              </Text>
            </View>
            <Icon name="chevron-right" size={20} color={colors.mutedForeground} style={styles.chevron} />
          </Pressable>
        ))}
      </ScrollView>
    </View>
  );
}

